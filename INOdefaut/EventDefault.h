
#ifndef EVENT_H
#define EVENT_H

#include <Arduino.h>
#include <stdint.h>

typedef uint8_t *Event;

inline Event createEvent(int size);
inline Event createEvent(int size, int value);
inline Event createEvent(uint8_t *data);
inline void deleteEvent(Event &event);

inline void setBit(Event &event, int position, bool value);
inline bool getBit(const Event event, int position);

inline Event bitwiseAnd(const Event &event1, const Event &event2);
inline Event bitwiseOr(const Event &event1, const Event &event2);
inline Event bitwiseXor(const Event &event1, const Event &event2);
inline Event bitwiseNot(const Event &event);
inline bool areEqual(const Event &event1, const Event &event2);

// ADD-ALL-EVENTS

#endif