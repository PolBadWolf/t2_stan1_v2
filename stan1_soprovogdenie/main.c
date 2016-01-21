
#include "main.h"

// часы для отладки
unsigned int t_msec;
unsigned char t_sec;
unsigned char t_min;
unsigned char t_chas;
// ошибка
unsigned int error_code;
unsigned int error_data;

// цикл колесо
unsigned char koleso_cicle;

// индикация дефектов
unsigned char ind_def8[8];
unsigned char ind_def12[12];
unsigned char ind_def_k=170;

// регистр для сбора дефектов перед линеей реза
unsigned char def_rez_m[16];
unsigned char def_rez=0;
unsigned char def_rez_out=0;

// индикация труб после трубо отрезного станка
unsigned char dt_12=0;
unsigned char dt_23=0;
unsigned char dt_34=0;
unsigned char dt_45=0;

// флаг индикации состояния упора
unsigned char upor_flag=0;

// массив расстояний до точек слежения ( дефектоскопы, краскопульты )
// unsigned int device_s[20];

unsigned int tube_count;
unsigned int tube_count_def;

// сигнализация работы датциков сопровождения трубы
unsigned int sign_count_d1;
unsigned int sign_count_d2;
unsigned int sign_count_d3;
unsigned int sign_count_d4;
unsigned int sign_count_d5;
// максимальная выдержка между сработками датчиков
unsigned int sign_dl=450;

// длина трубы в папугаях
unsigned int tube_len;

// флаг накопления
unsigned char sign_flag;


//----------------------------------------
// 12.08.2008
// работа с 3 краскопультом ( для тощиномера )
//
// время - длительность импульса покраски
// для переднего конца трубы
int kr3_dl_const;
int kr3_p_dl;
// время задержки  от переднего конца трубы ( м сек ) до заднего
int kr3_zd_const;
int kr3_zd;
// время - длительность импульса покраски
// для заднего конца трубы
int kr3_z_dl;

// меньше
char kr3_small_mask = 1<<5;
// больше
char kr3_big_mask = 1<<4;

void pusk_kr3( unsigned char defect )
{
    
    if ( (defect & kr3_small_mask)>0 )
        kr3_p_dl = kr3_dl_const;
    if ( (defect & kr3_big_mask)>0 )
        kr3_zd = kr3_zd_const;
}

void int_kr3(void)
{
    unsigned char flag_pokraski;
    flag_pokraski = 0;
    // ShowDigitZ(13,3,kr3_p_dl);
    // покраска, передний конец
    // толщина меньше нормы
    if ( kr3_p_dl>0 )
    {
        kr3_p_dl--;
        // вкл.
        flag_pokraski = 255;
    }
    // поиск заднего конца
    if ( kr3_zd==1 )
        kr3_z_dl = kr3_dl_const;
    if ( kr3_zd>0 )
        kr3_zd--;
    // покраска, передний конец
    // толщина меньше нормы
    if ( kr3_z_dl>0 )
    {
        kr3_z_dl--;
        flag_pokraski = 255;
    }
    // покраска
    if ( flag_pokraski==0 )
    {
        // настройка.
        // выкл.
        DDRF |= (1<<3);
        PORTF |= (1<<3);
    }
    else
    {
        // вкл.
        DDRF |= (1<<3);
        PORTF &= ~(1<<3);
    }
}

//-----------------------------------------

void main(void)
{
    // настройка таймера
    init_timer0();
    // настройка LCD
    InitLcd();
    // настройка индикации
    indicator_init();
    // глобальная настройка портов входных/выходных развязок
    {
        // выходная развязка - выход, на выход=1
        DDRF = 255;
        PORTF = 255;
        DDRE |= 1<<2;
        DDRE |= 1<<3;
        PORTE |= 1<<2;
        PORTE |= 1<<3;
        // верхняя входная развязка, вход
        DDRD &= ~(1<<5);
        DDRD &= ~(1<<4);
        DDRB &= ~(1<<7);
        DDRB &= ~(1<<6);
        DDRB &= ~(1<<5);
        DDRB &= ~(1<<4);
        DDRE &= ~(1<<7);
        DDRE &= ~(1<<6);
        DDRE &= ~(1<<5);
        DDRE &= ~(1<<4);
        PORTD |= 1<<5;
        PORTD |= 1<<4;
        PORTB |= 1<<7;
        PORTB |= 1<<6;
        PORTB |= 1<<5;
        PORTB |= 1<<4;
        PORTE |= 1<<7;
        PORTE |= 1<<6;
        PORTE |= 1<<5;
        PORTE |= 1<<4;
        // нижняя входная развязка, вход
        DDRC = 0;
        DDRD &= ~(1<<7);
        DDRD &= ~(1<<6);
        PORTC = 255;
        PORTD |= 1<<7;
        PORTD |= 1<<6;
    }
    // зануление сбора дефектов на линии реза
    {
        unsigned char i;
        for (i=0;i<16;i++)
            def_rez_m[i]=0;
    }
    // зануление сопровождения на станке
    rez_init();
    def_rez=0;
    // зануление работы упора и сопровождения по датчикам
    upor_init();
    // разрешение прерываний
    _SEI();
    //------------  
    {
        extern unsigned int timeout_key;
        unsigned char err;
        ClearDisplay1();
        Show_m2( 0,"Т2,стан№1");
        Show_m2(16,"29.01.2013");
        for (timeout_key=5000;;)
        {
            if ( timeout_key<5 )
                break;
        }
        err=PING & 0x08;
        // стереть память
        if ( err==0 )
            ep_zero();
    }

    //--------------
    // настройка цифрового фильтра датчиков дефектов
    defect_init();
    // начальная настройка kk
    pulsar_init();
    //-------------
    ClearDisplay1();
#ifdef RSPC
    rspc_init();
#endif
    // сброс кол-ва циклов сработки колеса
    koleso_cicle=0;
    for(;;)
    {
        //
        OutPC_FromMainCycle();
        // выбор экрана
        // indicator_next();
        // индикация
        indicator_cicle();
        //==========================
        if ( koleso_cicle>2 )
        {
            DDRF |= (1<<6);
            PORTF &= ~(1<<6);
        }
        //=========================================
        // Сигнал рез, разделение на трубы
        if ( rez_fg==255 )
        {
            // сброс флага рез
            rez_fg=0;
            // новая труба, длина=1, сброс дефектов
            rez_ms_def[0]=0;
            rez_ms_ind[0]=1;
            // перенос дефекта и трубы в следущую позитцию
            // 1-4 - сопровождение на станке от 1 до 4 труб
            // 1 - еще неотрезаная труба
            // перенос дефекта
            rez_ms_def[4]=rez_ms_def[3];
            rez_ms_def[3]=rez_ms_def[2];
            rez_ms_def[2]=rez_ms_def[1];
            rez_ms_def[1]=rez_ms_def[0];
            // перенос длины от линии реза
            rez_ms_ind[4]=rez_ms_ind[3];
            rez_ms_ind[3]=rez_ms_ind[2];
            rez_ms_ind[2]=rez_ms_ind[1];
            rez_ms_ind[1]=rez_ms_ind[0];
            rez_ms_ind[1]=1;
            // длина отрезаной трубы
            tube_len=rez_ms_ind[2];
            // сброс флага дефекта очередной трубы
            def_rez_out=0;
            // подсчет труб дефектных и общее количество
            tube_count++;
            {
            unsigned char maska;
            maska = ((1<<0) | (1<<1) | (1<<3));
            if ( (rez_ms_def[2] & maska)>0 )
                tube_count_def++;
        }
    }



    // сработало колесо
    if ( koleso_cicle>0 )
    {
        //=========================================
        {
            // анализ сигнала с кельвина и укладка маркировки ограниченой длины
            void kelv_analiz(void);
            kelv_analiz();
        }
        //=========================================
 
        // протяжка дефектов
        {
            unsigned int i;
            for ( i=1;i<8;i++)
            {
                if ( ind_def8[i]>0 )
                {
                    ind_def8[i]=0;
                    ind_def12[ d_def_sm[i]/ind_def_k ]=ind_def_k;
                }
        }
      
      
        // отображение на 12
        if ( ind_def12[0]>0 ) ind_def12[0]--;
        for (i=1;i<12;i++)
      {
          if ( ind_def12[i]>0 ) ind_def12[i]--;
          if ( d_def[indx_norm(i*ind_def_k)]>0 )  ind_def12[i-1]=ind_def_k;
      }
      //===============================================
      d_def[indx_norm(0)]=0;
      indx_inc();
      }

//=========================================
      // сопровождение от краско отметчиков до линии реза
      {
        unsigned char i;
        for (i=152;i>0;i--) s_def[i]=s_def[i-1];
        s_def[0]=d_def[indx_norm(1)];
      }
      //----------------------------
      // индикация дефекта от кк до реза
      if ( ind_d_korez>0 ) ind_d_korez--;
      if ( s_def[0]>0 ) ind_d_korez=d_rez_sm;
//=========================================
      // "пятно" дефектов в р-не линии реза
      /*
      {
        unsigned char i;
        // 
        for (i=0;i<10;i++)
        {
          // def_rez_m[i]=def_rez_m[i-1];
          // 105 -5 = 100
          // def_rez |= s_def[i+100];
          def_rez |= s_def[i+d_rez_sm-5];
          // def_rez |= def_rez_m[i];
        }
        // def_rez_m[1]=s_def[105-5];
      }
      */
      def_rez |= s_def[d_rez_sm];
      // ShowDigitZ(13,3,s_def[110]);
//==========================================

      // сопровождение на станке до 4 труб
      {
        unsigned char i;
        for (i=1;i<5;i++)
        {
          if ( rez_ms_ind[i]>0 )rez_ms_ind[i]++;
          if ( rez_ms_ind[i]>=rez_end )
          {
            // конец станка
            def_rez_out|=rez_ms_def[i];
            // сброс сопровождения на станке
            rez_ms_ind[i]=0;
            rez_ms_def[i]=0;
          }
        }
      }
//==============================================
      // 1 - накопление дефекта на неотрезаной трубе
      rez_ms_def[1]|=def_rez;
      // сброс флага дефектов
      def_rez=0;
//================================================
// сигнализация работы датчиков
      sign_flag=0;
      if ( sign_count_d1<sign_dl ) sign_count_d1++;
      else sign_flag|=(1<<0);
      if ( sign_count_d2<sign_dl ) sign_count_d2++;
      else sign_flag|=(1<<1);
      if ( sign_count_d3<sign_dl ) sign_count_d3++;
      else sign_flag|=(1<<2);
      if ( sign_count_d4<sign_dl ) sign_count_d4++;
      else sign_flag|=(1<<3);
      if ( sign_count_d5<sign_dl ) sign_count_d5++;
      else sign_flag|=(1<<4);
      if ( sign_flag>0 )
      {
          DDRF |= (1<<4);
          PORTF &= ~(1<<4);
      }
      else
      {
          DDRF |= (1<<4);
          PORTF |=  (1<<4);
      }
//==============================================
      // Работа с краскопультом
      pulsar_cicle();
      // конец цикла колесо
      koleso_cicle--;
    }
// ===== конец работы "внутри срабатывания колеса" =======
//--------------------
      // сопровождение по датчикам
      // датчик 1 ( фрсу )
      if ( trub_fg_1==255 )
      {
        trub_fg_1=128;
        // чтение дефекта
        defect1=def_rez_out;
        defect1r=defect1;
        // сброс флага дефекта очередной трубы
        // def_rez_out=0;
        // запуск краскоотметчика на толщиномер
        pusk_kr3( defect1r );
      }
      if ( trub_fg_1==0 )
      {
        trub_fg_1=128;
        defect1=0;
      }
//----------------------------------------
      // датчик 2
      if ( trub_fg_2==255 )
      {
        trub_fg_2=128;
        defect2=defect1r;
        defect2r=defect2;
      }
      if ( trub_fg_2==0 )
      {
        trub_fg_2=128;
        defect2=0;
      }
//---------------------------------------
      // датчик 3
      if ( trub_fg_3==255 )
      {
        trub_fg_3=128;
        if ( (sign_flag & (1<<1) ) == 0 )
        {
            defect3=defect2r;
            defect3r=defect3;
        }
        else
        {
            defect3=defect1r;
            defect3r=defect3;
        }
      }
      if ( trub_fg_3==0 )
      {
        trub_fg_3=128;
        defect3=0;
      }
//----------------------------------------
      // датчик 4
      if ( trub_fg_4==255 )
      {
        trub_fg_4=128;
        defect4=defect3r;
        defect4r=defect4;
      }
      if ( trub_fg_4==0 )
      {
        trub_fg_4=128;
        defect4=0;
      }
//----------------------------------------------
    {
    unsigned char mask_d;
    mask_d = ((1<<0) | (1<<1) | (1<<2) | (1<<3));
      // датчик 5
      if ( trub_fg_5==255 )
      {
        trub_fg_5=128;
        defect5=defect4r;
        // упор верх
        if ( (defect5 & mask_d)>0 )
        {
            upor_up();
            upor_flag=255;
        }
      }
      if ( trub_fg_5==0 )
      {
        trub_fg_5=128;
        // упор вниз
        if ( (defect5 & mask_d)>0 )
        {
            upor_dn();
            upor_flag=0;
        }
        defect5=0;
      }
    }
//===============================================
// Сигнализация
      if ( trub_fg_1i != 128 )
      {
          trub_fg_1i=128;
          sign_count_d1=0;
      }
      if ( trub_fg_2i != 128 )
      {
          trub_fg_2i=128;
          sign_count_d2=0;
      }
      if ( trub_fg_3i != 128 )
      {
          trub_fg_3i=128;
          sign_count_d3=0;
      }
      if ( trub_fg_4i != 128 )
      {
          trub_fg_4i=128;
          sign_count_d4=0;
      }
      if ( trub_fg_5i != 128 )
      {
          trub_fg_5i=128;
          sign_count_d5=0;
      }
//===============================================
  }
}
