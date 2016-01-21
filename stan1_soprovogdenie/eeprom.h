
#ifndef eeprom__h
#define eeprom__h

// OutEepromString( addr, len, Variable );
void ReadEepromString( unsigned char addr, unsigned char len, unsigned char *string );
void WriteEepromString( unsigned char addr, unsigned char len, unsigned char *string );

// Прочитать байт из EEPROM
unsigned char ReadEeprom(unsigned char addr);
// Записать байт в EEPROM
void WriteEeprom(unsigned char addr,  unsigned char data);

#endif
