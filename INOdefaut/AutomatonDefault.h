#ifndef AUTOMATON_H
#define AUTOMATON_H

#include <Arduino.h>

typedef void (*GenericAction)();

class Automaton
{
public:
    Automaton(int numStates, long *enabledEventStates, int (*MakeTransition)(int state, long event), void (*Loop)(int state));
    ~Automaton();

    int getActualState();           // Returns the current state of the automaton
    int getNumStates();             // Returns the total number of states in the automaton
    long getEnabledEvent();         // Returns the value of enabled events for the automaton
    void setActualState(int state); // Sets the current state of the automaton
    void setEvent(long event);      // Sets the value of the current event of the automaton
    long *enabledEventStates;                     // Pointer to the list of enabled event values
    int (*MakeTransition)(int state, long event); // Pointer to the state transition function
    void (*Loop)(int state);                      // Pointer to the current state execution function

private:
    int actualState = 0;                          // Current state of the automaton
    long actualEvent;                             // Value of the current event of the automaton
    int numStates;                                // Total number of states in the automaton
};
void setupPin();
long getEventControllablble(); // Obtains Controllablble events
long getEventUncontrollable(); // Obtains Uncontrollablble events

// ADD-AUTOMATON-LOOP
// ADD-EVENT-UNCONTROLLABLE
// ADD-STATE-ACTION
// ADD-TRANSITION-LOGIC

#endif