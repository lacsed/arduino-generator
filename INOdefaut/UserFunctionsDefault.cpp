#include "AutomatonDefault.h"

long getEventControllablble()
{
    if (!Serial.available())
    {
        return 0;
    }
    long valorSerial = Serial.parseInt();
    return valorSerial;
}

// ADD-GET-EVENT-UNCONTROLLABLE
void setupPin(){
    
}
 
// ADD-EVENT-UNCONTROLLABLE
// ADD-STATE-ACTION