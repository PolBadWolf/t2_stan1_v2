
#ifndef rez__h
#define rez__h

// флаг сработки датчика реза
extern unsigned char rez_fg;
extern unsigned char rez_fg1;
// тригер состояния датчика реза
extern unsigned char rez_tr;

// масивы :
// массив дефектов
extern unsigned char rez_ms_def[];
// массив начала трубы
extern unsigned int rez_ms_ind[];
// в разах длина от линии реза до датчика
extern unsigned int rez_end;

// чтение датчика
unsigned char rez_test(void);

void rez_int(void);
// настройка - сброс
void rez_init(void);

#endif
