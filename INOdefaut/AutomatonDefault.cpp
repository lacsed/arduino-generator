#include "AutomatonDefault.h"

Automaton::Automaton(int numStates, unsigned long *enabledEventStates, int (*MakeTransition)(int state, unsigned long event), void (*Loop)(int state))
    : numStates(numStates), enabledEventStates(enabledEventStates), MakeTransition(MakeTransition), Loop(Loop) {}

Automaton::~Automaton() {}

void Automaton::setEvent(unsigned long event)
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

unsigned long Automaton::getEnabledEvent()
{
    if (actualState >= 0 && actualState < sizeof(enabledEventStates) / sizeof(enabledEventStates[0]))
    {
        return enabledEventStates[actualState];
    }
    else
    {
        return 0;
    }
}

// ADD-SET-VECTOR

// ADD-AUTOMATON-LOOP

// ADD-TRANSITION-LOGIC
