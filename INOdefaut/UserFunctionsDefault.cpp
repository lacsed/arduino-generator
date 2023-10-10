#include "AutomatonDefault.h"

void setupPin()
{
}

void getEventControllable(Event &eventControllable)
{
    int totalEvent = sizeof(eventControllable) / sizeof(uint8_t);

    if (!Serial.available())
    {
        return;
    }

    String input = Serial.readStringUntil('\n'); 
    input.trim();


    int actualEvent = input.toInt();

    if (actualEvent >= 0 && actualEvent < totalEvent)
    {
        setBit(eventControllable, actualEvent, true);
    }
    else
    {
        Serial.println("There isn't this event or the input is not valid");
    }
}

// ADD-GET-EVENT-UNCONTROLLABLE

// ADD-EVENT-UNCONTROLLABLE

// ADD-STATE-ACTION