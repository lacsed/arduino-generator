#include "AutomatonDefaut.h"
#include "EventDefault.h"

// Define the number of automatons
#define NUMBER_AUTOMATON 1
#define NUMBER_SUPERVISOR 1
#define NUMBER_EVENT 1
#define SIZE_EVENT 1

// Functions
void executeTransition(uint8_t position);

// Initialize Events
int firstEventUncontrollable = 0;
int firstEventControllable = 0;

Event eventControllable = createEvent(SIZE_EVENT);
Event eventUncontrollable = createEvent(SIZE_EVENT);
Event emptyEvent = createEvent(SIZE_EVENT);

// Create a vector to store the automatons
Automaton automata[NUMBER_AUTOMATON];
Automaton supervisor[NUMBER_SUPERVISOR];

void setup()
{ // Initialize the automatons
// ADD-VECTOR-EVENT

// ADD-INSTANCE-AUTOMATON

    Serial.begin(9600);
    setupPin();
}

// Execute the main cycle
void loop()
{

    // Get the system events
    getEventControllable(eventControllable);
    getEventUncontrollable(eventUncontrollable);

    // Verify the events that are able to happen
    Event eventEnabled = createEvent(SIZE_EVENT);

    // Check the enabled events
    for (int i = 0; i < NUMBER_AUTOMATON; i++)
    {
        eventEnabled = bitwiseOr(eventEnabled, automata[i].getEnabledEvent(emptyEvent));
    }

    Event eventEnabledUncontrollable = bitwiseAnd(eventUncontrollable, eventEnabled);
    Event eventEnabledControllable = bitwiseAnd(eventControllable, eventEnabled);
    deleteEvent(eventEnabled);

    if (!areEqual(eventEnabledUncontrollable, emptyEvent))
    {
        for (int i = firstEventUncontrollable; i < NUMBER_EVENT; i++)
        {
            // Get one Uncontrollable Event each time
            bool existEvent = getBit(eventEnabledUncontrollable, i);
            if (existEvent)
            {
                executeTransition(i);
                setBit(eventUncontrollable, i, false);
                firstEventUncontrollable = (firstEventUncontrollable + 1) % NUMBER_EVENT;

                break;
            }
        }
    }
    else
    {
        eventEnabled = createEvent(SIZE_EVENT);

        // Check the enabled events
        for (int i = 0; i < NUMBER_AUTOMATON; i++)
        {
            eventEnabled = bitwiseOr(eventEnabled, supervisor[i].getEnabledEvent(emptyEvent));
        }

        // Check if any enabled Controllable event was detected
        eventEnabledControllable = bitwiseAnd(eventEnabledControllable, eventEnabled);

        if (!areEqual(eventEnabledControllable, emptyEvent))
        {

            for (int i = firstEventControllable; i < NUMBER_EVENT; i++)
            {
                // Get one Controllable Event each time
                bool existEvent = getBit(eventEnabledControllable, i);

                if (existEvent)
                {
                    executeTransition(i);
                    setBit(eventControllable, i, false);
                    firstEventControllable = (firstEventControllable + 1) % NUMBER_EVENT;
                    break;
                }
            }
        }

        deleteEvent(eventEnabled);
    }

    deleteEvent(eventEnabledUncontrollable);
    deleteEvent(eventEnabledControllable);

    // Execute the Loop function for each automaton
    for (int i = 0; i < NUMBER_AUTOMATON; i++)
    {
        int actualState = automata[i].getActualState();
        automata[i].Loop(actualState);
    }
}

void executeTransition(uint8_t position)
{
    // Execute the state transition for each supervisor
    for (int k = 0; k < NUMBER_SUPERVISOR; k++)
    {
        int actualState = supervisor[k].getActualState();
        int nextState = supervisor[k].MakeTransition(actualState, position);
        supervisor[k].setActualState(nextState);
    }

    // Execute the state transition for each automaton
    for (int j = 0; j < NUMBER_AUTOMATON; j++)
    {
        int actualState = automata[j].getActualState();
        int nextState = automata[j].MakeTransition(actualState, position);
        automata[j].setActualState(nextState);
    }
}
