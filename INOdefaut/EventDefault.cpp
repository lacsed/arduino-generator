
#include "EventDefault.h"

Event createEvent()
{
  Event event;
  zeroEvent(event);
  return event;
}

Event createEventFromData(uint8_t *data)
{
  Event event;
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    event.data[i] = data[i];
  }
  return event;
}

void copyEvent(const Event source, Event &destination)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    destination.data[i] = source.data[i];
  }
}

void setBit(Event &event, int position, bool value)
{
    int byteIndex = SIZE_EVENT- 1 - position / 8;
    int bitIndex = position % 8;

    if (value)
    {
        event.data[byteIndex] |= (1 << bitIndex);
    }
    else
    {
        event.data[byteIndex] &= ~(1 << bitIndex);
  }
}

bool getBit(const Event event, int position)
{
    int byteIndex = SIZE_EVENT- 1 - position / 8;
    int bitIndex = position % 8;

    return (event.data[byteIndex] >> bitIndex) & 1;
}

void bitwiseAnd(Event &result, const Event &event1, const Event &event2)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    result.data[i] = event1.data[i] & event2.data[i];
  }
}

void bitwiseOr(Event &result, const Event &event1, const Event &event2)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    result.data[i] = event1.data[i] | event2.data[i];
  }
}

void bitwiseXor(Event &result, const Event &event1, const Event &event2)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    result.data[i] = event1.data[i] ^ event2.data[i];
  }
}

void bitwiseNot(Event &result, const Event &event)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    result.data[i] = ~event.data[i];
  }
}

bool areEqual(const Event &event1, const Event &event2)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    if (event1.data[i] != event2.data[i])
    {
      return false;
    }
  }
  return true;
}

void zeroEvent(Event &event)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    event.data[i] = 0;
  }
}

void oneEvent(Event &event)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    event.data[i] = 0xFF;
  }
}

void printEvent(const Event event)
{
  for (int i = 0; i < SIZE_EVENT; i++)
  {
    Serial.print("[");
    for (int j = 7; j >= 0; j--)
    {
      Serial.print((event.data[i] >> j) & 1);
      if (j > 0)
      {
        Serial.print("-");
      }
    }
    Serial.print("]");
    if (i < SIZE_EVENT - 1)
    {
      Serial.print(",");
    }
  }
  Serial.println();
}
