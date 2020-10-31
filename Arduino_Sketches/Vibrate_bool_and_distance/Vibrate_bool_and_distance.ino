int analogPin = 13;                // LED connected to digital pin 13
bool lightOn = false;

void setup()
{
  Serial.begin(9600);
  pinMode(analogPin, OUTPUT);      // sets the digital pin as output
  pinMode(LED_BUILTIN, OUTPUT);
}


void loop() {
 

  if(Serial.available())
  {
    byte c = Serial.read();
    
    Serial.println( c );
    analogWrite(analogPin, c );
    if(Serial.available())
  
    char c = Serial.read();
    if (c)
    {
      if(c == 'A')
      {
        lightOn = true;
      }
      else if(c == 'Z')
      {
        lightOn = false;
      }
      c = NULL;
    }
  }
  if(lightOn)
  {
    analogWrite(LED_BUILTIN, 100);
    Serial.println("on");
  }
  else
  {
    digitalWrite(LED_BUILTIN, LOW);
    Serial.println("off");
    
  }
  
  delay(33);        // delay in between reads for stability

    }
