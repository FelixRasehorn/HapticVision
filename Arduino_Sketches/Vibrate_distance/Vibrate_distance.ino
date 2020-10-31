int analogPin = 13;                // LED connected to digital pin 13

void setup()
{
  Serial.begin(9600);
  pinMode(analogPin, OUTPUT);      // sets the digital pin as output
}


void loop() {
 

  if(Serial.available())
  {
    byte c = Serial.read();
    
    Serial.println( c );
    analogWrite(analogPin, c );
    
    
  }
  
  delay(33);        // delay in between reads for stability
}
