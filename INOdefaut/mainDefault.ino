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
    long eventControllablble = getEventControllablble();
    long eventUncontrollable = getEventUncontrollable();
    long eventEnabled = (1 << NUMBER_EVENT) - 1;

    // Check the enabled events
    for (int i = 0; i < NUMBER_AUTOMATON; i++)
    {
        eventEnabled &= supervisor[i].getEnabledEvent();
    }

    // Check if any enabled event was detected
    long eventEnabledControlable = eventControllablble & eventEnabled;
    long eventEnabledUncontrollable = eventUncontrollable & eventEnabled;

    if (eventEnabledUncontrollable > 0)
    {
        for (int i = 0; i < NUMBER_EVENT; i++)
        {
            // Get the First Uncontrollable Event Enabled

            long event = eventEnabledUncontrollable & (1L<<i);
            if (event > 0)
            {
                // Execute the state transition for each automaton
                for (int j = 0; j < NUMBER_AUTOMATON; j++)
                {
                    int actualState = automata[j].getActualState();
                    int nextState = automata[j].MakeTransition(actualState, event);
                    automata[j].setActualState(nextState);
                }
                break;
            }
        }
    }
    else
    {

        if (eventEnabledControlable > 0)
        {

            for (int i = 0; i < NUMBER_EVENT; i++)
            {

                // Get the First Uncontrollable Event Enabled
                long event = eventEnabledControlable &(1L<<i);
                if (event > 0)
                {
                    // Execute the state transition for each automaton
                    for (int j = 0; j < NUMBER_AUTOMATON; j++)
                    {
                        int actualState = automata[j].getActualState();
                        int nextState = automata[j].MakeTransition(actualState, event);
                        automata[j].setActualState(nextState);
                    }
                    break;
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
