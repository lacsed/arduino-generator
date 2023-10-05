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
    eventControllable = bitwiseOr(eventControllable, getEventControllablble());
    eventUncontrollable = bitwiseOr(eventUncontrollable, getEventUncontrollable());

    // Verify the events that are able to happen
    Event eventEnabled = createEvent(SIZE_EVENT);

    // Check the enabled events
    for (int i = 0; i < NUMBER_AUTOMATON; i++)
    {
        eventEnabled = bitwiseOr(eventEnabled, automata[i].getEnabledEvent());
    }

    Event eventEnabledUncontrollable = bitwiseAnd(eventUncontrollable, eventEnabled);
    Event eventEnabledControllable = bitwiseAnd(eventControllable, eventEnabled);
    deleteEvent(eventEnabled);

    if (!areEqual(eventEnabledUncontrollable, emptyEvent))
    {
        for (int i = firstEventUncontrollable; i < NUMBER_EVENT; i++)
        {
            // Get one Uncontrollable Event each time
            Event genericEvent = createEvent(SIZE_EVENT);
            setBit(&genericEvent, i, true);

            genericEvent = bitwiseAnd(eventEnabledUncontrollable, genericEvent);

            if (!areEqual(genericEvent, emptyEvent))
            {
                executeTransition(i);

                setBit(eventUncontrollable, i, false);
                firstEventUncontrollable = (firstEventUncontrollable + 1) % NUMBER_EVENT;
                deleteEvent(genericEvent);
                break;
            }

            deleteEvent(genericEvent);
        }
    }
    else
    {
        eventEnabled = createEvent(SIZE_EVENT);

        // Check the enabled events
        for (int i = 0; i < NUMBER_AUTOMATON; i++)
        {
            eventEnabled = bitwiseOr(eventEnabled, supervisor[i].getEnabledEvent());
        }

        // Check if any enabled Controllable event was detected
        eventEnabledControllable = bitwiseAnd(eventEnabledControllable, eventEnabled);

        if (!areEqual(eventEnabledControllable, emptyEvent))
        {

            for (int i = firstEventControllable; i < NUMBER_EVENT; i++)
            {
                // Get one Uncontrollable Event each time
                Event genericEvent = createEvent(SIZE_EVENT);
                setBit(&genericEvent, i, true);

                genericEvent = bitwiseAnd(eventEnabledControllable, genericEvent);

                if (!areEqual(genericEvent, emptyEvent))
                {
                    executeTransition(genericEvent);

                    setBit(eventControllable, i, false);
                    firstEventControllable = (firstEventControllable + 1) % NUMBER_EVENT;
                    deleteEvent(genericEvent);
                    break;
                }

                deleteEvent(genericEvent);
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
