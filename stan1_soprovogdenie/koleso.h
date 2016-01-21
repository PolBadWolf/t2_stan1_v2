// датчик колесо :
// счетчик для устранения дребезга
extern unsigned char d_koleso_count;
// верхний предел счетсчика
extern unsigned char d_koleso_up;
// тригер состояние датчика колеса
extern unsigned char d_koleso_tr;
// внешний флаг сработки датчика колеса
extern unsigned char d_koleso_fg;
// внутренний флаг сработки датчика колеса для передачи в компьютер
extern unsigned char d_koleso_fg2;


// отработка датчика колеса
void koleso_int(void);

unsigned char koleso_test(void);

// on & off
void circle_view(void);
void circle_tah(void);

// on & off = speed
// состояние в прошлый раз
extern unsigned char circle_test;
// счетчик
extern unsigned char circle_count;
// в включеном состоянии
extern unsigned char circle_count_on;
// в выключеном состоянии
extern unsigned char circle_count_off;
// скорость стана
extern unsigned int circle_speed;


