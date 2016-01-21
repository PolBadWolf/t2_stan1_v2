
#ifndef pulsar__h
#define pulsar__h

// цикл проверки дефекта
void pulsar_cicle(void);
// от системного таймера ( для сигнала работа стана )
void pulsar_int(void);
// начальная настройка
void pulsar_init(void);


// тригер состояние датчиков дефектов
extern unsigned char work_tr;
// событие состояния датчика работы стана
extern unsigned char work_fg;

// массив сохранения расстояния до КК
extern unsigned int kk_sm[20];
// 8 - дефект
// 9 - стык

#endif
