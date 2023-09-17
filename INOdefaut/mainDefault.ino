#include "AutomatonDefaut.h"

// Define the number of automatons
#define NUMBER_AUTOMATON 1
#define NUMBER_SUPERVISOR 1
#define NUMBER_EVENT 1

// Initialize the automatons
// ADD-VECTOR-EVENT

// Create a vector to store the automatons
// ADD-INSTANCE-AUTOMATON

void setup()
{
    Serial.begin(9600);
    setupPin();
}

// Execute the main cycle
void loop()
{

    // Get the system events
    unsigned long eventControllablble = getEventControllablble();
    unsigned long eventUncontrollable = getEventUncontrollable();

    if (eventUncontrollable > 0)
    {
        for (int i = 0; i < NUMBER_EVENT; i++)
        {
            // Get one Uncontrollable Event each time

            unsigned long event = eventUncontrollable & (1L << i);
            if (event > 0)
            {
                // Execute the state transition for each supervisor
                for (int k = 0; k < NUMBER_SUPERVISOR; k++)
                {
                    int actualState = supervisor[k].getActualState();
                    int nextState = supervisor[k].MakeTransition(actualState, event);
                    supervisor[k].setActualState(nextState);
                }

                // Execute the state transition for each automaton
                for (int j = 0; j < NUMBER_AUTOMATON; j++)
                {
                    int actualState = automata[j].getActualState();
                    int nextState = automata[j].MakeTransition(actualState, event);
                    automata[j].setActualState(nextState);
                }
            }
        }
    }

    unsigned long eventEnabled = (1 << NUMBER_EVENT) - 1;

    // Check the enabled events
    for (int i = 0; i < NUMBER_SUPERVISOR; i++)
    {
        eventEnabled &= supervisor[i].getEnabledEvent();
    }

    // Check if any enabled Controllable event was detected
    unsigned long eventEnabledControlable = eventControllablble & eventEnabled;

    if (eventEnabledControlable > 0)
    {

        for (int i = 0; i < NUMBER_EVENT; i++)
        {

            // Get the First Controllable Event Enabled
            unsigned long event = eventEnabledControlable & (1L << i);
            if (event > 0)
            {
                // Execute the state transition for each supervisor
                for (int k = 0; k < NUMBER_SUPERVISOR; k++)
                {
                    int actualState = supervisor[k].getActualState();
                    int nextState = supervisor[k].MakeTransition(actualState, event);
                    supervisor[k].setActualState(nextState);
                }

                // Execute the state transition for each automaton
                for (int j = 0; j < NUMBER_AUTOMATON; j++)
                {
                    int actualState = automata[j].getActualState();
                    int nextState = automata[j].MakeTransition(actualState, event);
                    automata[j].setActualState(nextState);
                }
            }
        }
    }

    // Execute the Loop function for each automaton
    for (int i = 0; i < NUMBER_AUTOMATON; i++)
    {
        int actualState = automata[i].getActualState();
        automata[i].Loop(actualState);
    }
}
