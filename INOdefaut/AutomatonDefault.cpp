#include "AutomatonDefault.h"
#include "EventDefault.h"

Automaton::Automaton(int numStates, Event *enabledEventStates, int (*MakeTransition)(int state, Event eventOccurred), void (*Loop)(int state))
    : numStates(numStates), enabledEventStates(enabledEventStates), MakeTransition(MakeTransition), Loop(Loop) {}

Automaton::Automaton() {}

Automaton::~Automaton() {}

void Automaton::setEvent(Event event)
{
    actualEvent = event;
}

void Automaton::setActualState(int state)
{
    actualState = state;
}

int Automaton::getActualState()
{
    return actualState;
}

int Automaton::getNumStates()
{
    return numStates;
}

Event Automaton::getEnabledEvent()
{
    return enabledEventStates[actualState];
}

// ADD-GET-EVENT-CONTROLLABLE

// ADD-GET-EVENT-UNCONTROLLABLE

// ADD-SET-VECTOR

// ADD-AUTOMATON-LOOP

// ADD-TRANSITION-LOGIC
