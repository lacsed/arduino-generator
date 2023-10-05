
#include "EventDefault.h"

inline Event createEvent(int size)
{
    Event event = new uint8_t[size];
    memset(event, 0, size);
    return event;
}

inline Event createEvent(int size, int value)
{
    Event event = new uint8_t[size];
    memset(event, value, size);
    return event;
}

inline Event createEvent(uint8_t *data)
{
    int size = sizeof(data);
    Event event = new uint8_t[size];
    memcpy(event, data, size);
    return event;
}

inline void deleteEvent(Event $event)
{
    delete[] event;
}

inline void setBit(Event &event, int position, bool value)
{
    int byteIndex = position / 8;
    int bitIndex = position % 8;
    if (value)
    {
        event[byteIndex] |= (1 << bitIndex);
    }
    else
    {
        event[byteIndex] &= ~(1 << bitIndex);
    }
}

inline bool getBit(const Event event, int position)
{
    int byteIndex = position / 8;
    int bitIndex = position % 8;
    return (event[byteIndex] >> bitIndex) & 1;
}

inline Event bitwiseAnd(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] & event2[i];
    }
    return result;
}

inline Event bitwiseOr(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] | event2[i];
    }
    return result;
}

inline Event bitwiseXor(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] ^ event2[i];
    }
    return result;
}

inline Event bitwiseNot(const Event &event)
{
    int size = sizeof(event);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = ~event[i];
    }
    return result;
}

inline bool areEqual(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    if (sizeof(event2) != size)
    {
        return false;
    }

    return memcmp(event1, event2, size) == 0;
}
