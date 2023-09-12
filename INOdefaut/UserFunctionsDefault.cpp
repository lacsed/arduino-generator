#include "AutomatonDefault.h"

unsigned long getEventControllablble()
{
    if (!Serial.available())
    {
        return 0;
    }
    unsigned long valorSerial = Serial.parseInt();
    return valorSerial;
}

// ADD-GET-EVENT-UNCONTROLLABLE
void setupPin(){
    
}
 
// ADD-EVENT-UNCONTROLLABLE
// ADD-STATE-ACTION