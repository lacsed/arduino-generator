#include "AutomatonDefault.h"

Event getEventControllablble()
{
    if (!Serial.available())
    {
        return 0;
    }
    Event valorSerial = createEvent(Serial.parseInt());
    return valorSerial;
}

// ADD-GET-EVENT-UNCONTROLLABLE
void setupPin(){
    
}
 
// ADD-EVENT-UNCONTROLLABLE
// ADD-STATE-ACTION