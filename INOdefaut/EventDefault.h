
#ifndef EVENT_H
#define EVENT_H

#include <Arduino.h>
#include <stdint.h>

typedef uint8_t *Event;

Event createEvent(int size);
Event createEventFromData(uint8_t *data);
void deleteEvent(Event &event);

void setBit(Event &event, int position, bool value);
bool getBit(const Event event, int position);

Event bitwiseAnd(const Event &event1, const Event &event2);
Event bitwiseOr(const Event &event1, const Event &event2);
Event bitwiseXor(const Event &event1, const Event &event2);
Event bitwiseNot(const Event &event);
bool areEqual(const Event &event1, const Event &event2);

// ADD-ALL-EVENTS

#endif