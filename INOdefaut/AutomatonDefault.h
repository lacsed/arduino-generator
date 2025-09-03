
#ifndef AUTOMATON_H
#define AUTOMATON_H

#include "Event.h"

typedef void (*GenericAction)();

class Automaton
{
public:
    Automaton(int numStates, Event *enabledEventStates, int (*MakeTransition)(int state, Event eventOccurred), void (*Loop)(int state));
    Automaton();
    ~Automaton();

    int getActualState();                                  // Returns the current state of the automaton
    int getNumStates();                                    // Returns the total number of states in the automaton
    Event getEnabledEvent();                               // Returns the value of enabled events for the automaton
    void setActualState(int state);                        // Sets the current state of the automaton
    void setEvent(Event event);                            // Sets the value of the current event of the automaton
    Event *enabledEventStates;                             // Pointer to the list of enabled event values
    int (*MakeTransition)(int state, Event eventOccurred); // Pointer to the state transition function
    void (*Loop)(int state);                               // Pointer to the current state execution function

private:
    int actualState = 0; // Current state of the automaton
    Event actualEvent;   // Value of the current event of the automaton
    int numStates;       // Total number of states in the automaton
};

void setupPin();
void doEveryLoop();
void getEventControllable(Event &eventControllable);     // Obtains Controllablble events
void getEventUncontrollable(Event &eventUncontrollable); // Obtains Uncontrollablble events

// ADD-AUTOMATON-LOOP

// ADD-EVENT-CONTROLLABLE

// ADD-EVENT-UNCONTROLLABLE

// ADD-STATE-ACTION

// ADD-TRANSITION-LOGIC

#endif