
#include "EventDefault.h"

Event createEvent(int size)
{
    Event event = new uint8_t[size];
    memset(event, 0, size);
    return event;
}

Event createEventFromData(uint8_t *data)
{
    int size = sizeof(data);
    Event event = new uint8_t[size];
    memcpy(event, data, size);
    return event;
}

void deleteEvent(Event &event)
{
    delete[] event;
}

void setBit(Event &event, int position, bool value)
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

bool getBit(const Event event, int position)
{
    int byteIndex = position / 8;
    int bitIndex = position % 8;
    return (event[byteIndex] >> bitIndex) & 1;
}

Event bitwiseAnd(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] & event2[i];
    }
    return result;
}

Event bitwiseOr(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] | event2[i];
    }
    return result;
}

Event bitwiseXor(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = event1[i] ^ event2[i];
    }
    return result;
}

Event bitwiseNot(const Event &event)
{
    int size = sizeof(event);
    Event result = createEvent(size);
    for (int i = 0; i < size; i++)
    {
        result[i] = ~event[i];
    }
    return result;
}

bool areEqual(const Event &event1, const Event &event2)
{
    int size = sizeof(event1);
    if (sizeof(event2) != size)
    {
        return false;
    }

    return memcmp(event1, event2, size) == 0;
}
