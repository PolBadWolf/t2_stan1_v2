// Вывести строку на индикатор с заданной позиции
void Show_m(unsigned char pos, unsigned char *string)
{
 unsigned char *s;
 unsigned char tmp;
  s = display + pos;
  for(tmp=*string++;tmp>0;)
  {
    if ( tmp<4 ) mig_string=tmp-1;
    else
    {
      *s++ = tmp;
      if ( mig_string==1 )
      { if ( pos<n_str ) blinc_line1 |= flash_decoder[pos];
        else          blinc_line2 |= flash_decoder[pos];
      }
      if ( mig_string==0 )
      { if ( pos<n_str ) blinc_line1 &= ~flash_decoder[pos];
        else          blinc_line2 &= ~flash_decoder[pos];
      }
      pos++;
    }
    tmp = *string++;
  }
}

// Вывести строку на индикатор с заданной позиции
void Show_m1(unsigned char pos, unsigned char *string, unsigned char nach, unsigned char len )
{
 unsigned char *s;
 unsigned char *s1;
 unsigned char tmp;
 unsigned char count=0;
  s = display + pos;
  s1 = string + nach;
  for(tmp=*s1++;tmp>0;)
  {
    if ( tmp<4 ) mig_string=tmp-1;
    else
    {
      *s++ = tmp;
      if ( mig_string==1 )
      { if ( pos<n_str ) blinc_line1 |= flash_decoder[pos];
        else          blinc_line2 |= flash_decoder[pos];
      }
      if ( mig_string==0 )
      { if ( pos<n_str ) blinc_line1 &= ~flash_decoder[pos];
        else          blinc_line2 &= ~flash_decoder[pos];
      }
      pos++;
      count++;
    }
    tmp = *s1++;
    if ( count>=len ) tmp=0;
  }
}

// Вывести строку на индикатор с заданной позиции
void Show_m2(unsigned char pos, unsigned char *string)
{
 unsigned char *s;
 unsigned char *s1;
 unsigned char tmp;
  s = display + pos;
  s1 = string;
  for(tmp=*s1++;tmp>0;)
  {
    *s++ = tmp;
    pos++;
    tmp = *s1++;
  }
}

/* Вывести символ на индикатор с заданной позиции */
void ShowChar(unsigned char pos, unsigned char ch)
{
//if(pos < 32)
 display[pos] = ch;
}

/* Показать значение температуры в десятичном виде 00.0 в конкретной позиции */
void ShowTemperature(unsigned char pos, unsigned int digit)
{
 unsigned char *s;
 //unsigned int i;
 if((pos > 31) || ((pos + 4) > 32))
  return;
 if(digit == 0)
  {
   s = display + pos; /* Указатель на знакоместо в буфере */
   *s++ = ' '; /*  */
   *s++ = '0'; /*  */
   *s++ = '.'; /*  */
   *s = '0';
   return;
  }
 s = display + pos + 4; /* Указатель на знакоместо младшего разряда */
 *--s = digit % 10 + '0';
 *--s = '.';
 if(digit/10 > 0) *--s = (digit/10)%10 + '0';
 else *--s = '0'; /* Ведущие пробелы */
 if(digit/100 > 0) *--s = (digit/100)%10 + '0';
 else *--s = ' '; /* Ведущие пробелы */
}

/* Показать число с ведущими пробелами в десятичном виде в конкретной позиции с определенным количеством цифр */
void ShowDigit(unsigned char pos, unsigned char numdigit, unsigned long digit)
{
 unsigned char *s;
 unsigned long i;
if(digit == 0)
 {
  s = display + pos; /* Указатель на знакоместо в буфере */
  for(;numdigit >1; numdigit--)
   *s++ = ' '; /* Ведущие пробелы */
  *s = '0';
  return;
 }

s = display + pos + numdigit; // Указатель на знакоместо младшего разряда
*--s = digit % 10 + '0';

for(i = 10;numdigit >1;i*=10, numdigit--)
 if(digit/i > 0) *--s = (digit/i)%10 + '0';
 else *--s = ' '; // Ведущие пробелы

}

/* Показать число с ведущими нулями в десятичном виде в конкретной позиции с определенным количеством цифр */
void ShowDigitZ(unsigned char pos, unsigned char numdigit, unsigned long digit)
{
 unsigned char *s;
 unsigned long i;
if(digit == 0)
 {
  s = display + pos; /* Указатель на знакоместо в буфере */
  for(;numdigit >1; numdigit--)
   *s++ = '0'; /* Ведущие нули */
  *s = '0';
  return;
 }
s = display + pos + numdigit; /* Указатель на знакоместо младшего разряда */
*--s = digit % 10 + '0';
for(i = 10;numdigit >1;i*=10, numdigit--)
 if(digit/i > 0) *--s = (digit/i)%10 + '0';
 else *--s = '0'; /* Ведущие нули */
}

/* Указатель на изменяемый объект, размер объекта, минимальное значение, максимальное значение, признак перехода через минимальное значение */
void ParametrDown(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_min)
{
// ShowDigitZ(29,3,KEYB_REPEAT);
if(size == sizeof(unsigned int)) /* Если параметр типа unsigned int */
 {
  unsigned int * param = ptr;
  if(*param > min_param)
  { // Если флаг признака действия автоповтора установлен, то уменьшить параметр на 10 иначе на 1
   if(KEYB_REPEAT)
   {
    if(*param < 10)
    {
     if(over_min) *param = max_param;
     else         *param = min_param;
    }
    else
    {
     *param-=10;
     if(*param < min_param)
     {
      if(over_min) *param = max_param;
      else         *param = min_param;
     }
    }
   }
   else (*param)--;
  }
  else
  {
   if(over_min) *param = max_param;
   else         *param = min_param;
  }
 }
else /* Иначе параметр типа unsigned char */
 {
  unsigned char * param = ptr;
  if(*param > min_param)
  { // Если флаг признака действия автоповтора установлен, то уменьшить параметр на 10 иначе на 1
   if(KEYB_REPEAT)
   {
    if(*param < 10)
    {
     if(over_min) *param = max_param;
     else         *param = min_param;
    }
    else
    {
     *param-=10;
     if(*param < min_param)
     {
      if(over_min) *param = max_param;
      else         *param = min_param;
     }
    }
   }
   else (*param)--;
  }
  else
  {
   if(over_min) *param = max_param;
   else         *param = min_param;
  }
 }
SET_SAVE; /* Установить флаг "Сохранить изменения" */
}

/* Указатель на изменяемый объект, размер объекта, минимальное значение, максимальное значение, признак перехода через максимальное значение */
void ParametrUp(void *ptr,unsigned int size,unsigned int min_param,unsigned int max_param,unsigned char over_max)
{
// ShowDigitZ(29,3,KEYB_REPEAT);
if(size == sizeof(unsigned int)) /* Если параметр типа unsigned int */
 {
  unsigned int * param = ptr;
  if(*param < max_param)
  {  // Если флаг признака действия автоповтора установлен, то увеличить параметр на 10 иначе на 1
   if(KEYB_REPEAT)
   {
    if(*param > 0xFFF5)
    {
     if(over_max) *param = min_param;
     else         *param = max_param;
    }
    else
    {
     *param+=10;
     if(*param > max_param)
     {
      if(over_max) *param = min_param;
      else         *param = max_param;
     }
    }
   }
   else (*param)++;
  }
  else
  {
   if(over_max) *param = min_param;
   else         *param = max_param;
  }
 }
else /* Иначе параметр типа unsigned char */
 {
  unsigned char * param = ptr;
  if(*param < max_param)
  {  // Если флаг признака действия автоповтора установлен, то увеличить параметр на 10 иначе на 1
   if(KEYB_REPEAT)
   {
    if(*param > 0xF5)
    {
     if(over_max) *param = min_param;
     else         *param = max_param;
    }
    else
    {
     *param+=10;
     if(*param > max_param)
     {
      if(over_max) *param = min_param;
      else         *param = max_param;
     }
    }
   }
   else (*param)++;
  }
  else
  {
   if(over_max) *param = min_param;
   else         *param = max_param;
  }
 }
SET_SAVE; /* Установить флаг "Сохранить изменения" */
}


