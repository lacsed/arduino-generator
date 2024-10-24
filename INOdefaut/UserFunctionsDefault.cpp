#include "AutomatonDefault.h"

void setupPin()
{
}

// This function will be called every Arduino loop, put here something that you want to repeat always
void doEveryLoop(){
/*
    Example of reading events through the Serial port

    if (!Serial.available())
    {
        return;
    }

    String input = Serial.readStringUntil('\n');
    input.trim();

    int actualEvent = input.toInt();

*/
}

// This set of functions should be implemented in a way to handle the controllable events of the system
// ADD-EVENT-CONTROLLABLE


// This set of functions should be implemented in a way to handle the uncontrollable events of the system
// ADD-EVENT-UNCONTROLLABLE

// Here the expected actions should be implemented in each state of the system 
// ADD-STATE-ACTION