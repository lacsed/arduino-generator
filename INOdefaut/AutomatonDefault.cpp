#include "AutomatonDefault.h"

Automaton::Automaton(int numStates, long *enabledEventStates, int (*MakeTransition)(int state, long event), void (*Loop)(int state))
    : numStates(numStates), enabledEventStates(enabledEventStates), MakeTransition(MakeTransition), Loop(Loop) {}

Automaton::~Automaton() {}

void Automaton::setEvent(long event)
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

long Automaton::getEnabledEvent()
{
    return enabledEventStates[actualState];
}

// ADD-SET-VECTOR

// ADD-AUTOMATON-LOOP

// ADD-TRANSITION-LOGIC
