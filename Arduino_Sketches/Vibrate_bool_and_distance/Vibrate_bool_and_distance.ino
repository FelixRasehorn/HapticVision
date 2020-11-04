#define LED 3                // LED connected to digital pin 13

bool lightOn = false;
byte strength = 0;

void setup()
{
  Serial.begin(9600);
  pinMode(LED, OUTPUT);      // sets the digital pin as output
  pinMode(LED_BUILTIN, OUTPUT);
  Serial.setTimeout(10);
}


void loop() 
{  
  if(Serial.available())
  {
    strength = Serial.read(); //Serial.parseInt();
    
    if(strength)
    {
      if(strength > 1)
      {
        if(!lightOn) Serial.println("On");
        
        lightOn = true;
        analogWrite(LED, strength);
        Serial.print("Strength: ");
        Serial.println(strength); 
      }
      else
      {
        Serial.println("Off");
        lightOn = false;
        digitalWrite(LED, LOW);
      }
    }

    delay(33);
  }
}
