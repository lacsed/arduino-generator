

#ifndef EVENT_H
#define EVENT_H

#include <Arduino.h>
#include <stdint.h>

#define NUMBER_EVENT 1
#define SIZE_EVENT 1

struct Event
{
    uint8_t data[SIZE_EVENT];
};

Event createEvent();
Event createEventFromData(uint8_t *data);
void copyEvent(const Event source, Event &destination);

void setBit(Event &event, int position, bool value);
bool getBit(const Event event, int position);

void bitwiseAnd(Event &result, const Event &event1, const Event &event2);
void bitwiseOr(Event &result, const Event &event1, const Event &event2);
void bitwiseXor(Event &result, const Event &event1, const Event &event2);
void bitwiseNot(Event &result, const Event &event);

bool areEqual(const Event &event1, const Event &event2);

void zeroEvent(Event &event);
void oneEvent(Event &event);

void printEvent(const Event event);

// ADD-ALL-EVENTS

#endif