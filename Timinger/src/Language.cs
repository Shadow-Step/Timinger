using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Timinger
{
    public class Language
    {
        public Language()
        {

        }
        public Language(string local)
        {
            switch (local)
            {
                case "RUS":
                    SwitchToRUS();
                    break;
                case "ENG":
                    SwitchToENG();
                    break;
            }
        }

        public static string CurrentLanguage { get; set; } = "RUS";

        public static string Name { get; set; } = "Название";
        public static string Time { get; set; } = "Время";
        public static string Type { get; set; } = "Тип";
        public static string Card { get; set; } = "Карта";
        public static string Add { get; set; } = "Добавить";
        public static string Del { get; set; } = "Удалить";
        public static string Cards { get; set; } = "Карты";
        public static string Copy { get; set; } = "Копировать";

        public static string Army { get; set; } = "Армия";
        public static string Cap { get; set; } = "Капитан";

        public static string ArmyType_Army { get; set; } = "Ударка";
        public static string ArmyType_Captain { get; set; } = "Капитан";
        public static string ArmyType_Unknown { get; set; } = "Неизвестно";

        public static string RestTime { get; set; } = "Ожидание";
        public static string Discipline { get; set; } = "Дисциплина";
        public static string Recount { get; set; } = "Посчитать";
        public static string No { get; set; } = "Нет";
        public static string Active { get; set; } = "Активна";

        //Menu
        public static string File { get; set; } = "Файл";
        public static string Help { get; set; } = "Помощь";
        public static string Donate { get; set; } = "Поддержать проект";
        public static string Lang { get; set; } = "Язык";
        public static string New { get; set; } = "Новый";
        public static string Open { get; set; } = "Открыть";
        public static string Save { get; set; } = "Сохранить";
        public static string SaveAs { get; set; } = "Сохранить как";
        public static string Exit { get; set; } = "Выход";

        //GropBox
        public static string Targets { get; set; } = "Цели";
        public static string Target { get; set; } = "Цель";
        public static string Attacks { get; set; } = "Атаки";
        public static string Options { get; set; } = "Настройки";
        public static string Result { get; set; } = "Результат";
        public static string Unlock { get; set; } = "Разблокировать";

        public static string Card2x { get; set; } = "Карты х2";
        public static string Card3x { get; set; } = "Карты х3";
        public static string Card5x { get; set; } = "Карты х5";

        public static string Delta { get; set; } = "Дельта";
        public static string Text { get; set; } = "Текст";
        public static string Filters { get; set; } = "Фильтры";
        public static string TargetName { get; set; } = "Название цели";
        public static string AttackName { get; set; } = "Название атаки";
        public static string TypeArmy { get; set; } = "Тип армии";

        public static string TotalTimeToAttack { get; set; } = "Общее время для атаки";
        public static string ExitMessage { get; set; } = "Вы уверены что хотите выйти?";
        public static string CreateNewFile { get; set; } = "Несохраненные данные будут утеряны, продолжить?";
        public static string StatusBarText { get; set; } = "Не пропусти новый софт и свежие обновления ";
        public static string ForceCaptain { get; set; } = "Обязателен капитан";
        public static string NoSlotForCaptain { get; set; } = "Ни одна из атак не может быть капитаном. Измените тип атаки или выключите режим 'Обязателен капитан'";
        public static string NoVariants { get; set; } = "Не найдено ни одного подходящего варианта. Примените карты или уменьшите время ожидания";
        public static string UnsafeWarning { get; set; } = "Внимание! Изменение параметров может привести к погрешностям в вычислениях, перепроверяйте результаты!";
        public static string AboutProgram { get; set; } = "О программе";
        public static string ToolTipDelta { get; set; } = "По-умолчанию - 3.00";
        public static string CopyTextToBuffer { get; set; } = "Скопируйте текст в буфер обмена";
        public static string NotEnoughAttacks { get; set; } = "Требуется минимум 2 активные атаки.";
        public static string Abort { get; set; } = "Прервать";
        public static string Variants { get; set; } = "Вариантов";
        public static string FAQ { get; set; } = "FAQ";
        public static string Updates { get; set; } = "Обновления";
        public static string From { get; set; } = "Из";
        public static string SendAttack { get; set; } = "Подготовить:";

        public static string Timer { get; set; } = "Таймер";
        public static string TimerOptions { get; set; } = "Настройки таймера";
        public static string Accept { get; set; } = "Принять";
        public static string Cancel { get; set; } = "Отмена";
        public static string Volume { get; set; } = "Громкость";
        public static string HotKeyForStart { get; set; } = "Горячая клавиша для запуска таймера";
        public static string UseAttacksFromTable { get; set; } = "Использовать атаки из таблицы";
        public static string PlaySignalBefore { get; set; } = "Проиграть сигнал за";
        public static string SecondsToSending { get; set; } = "секунд до отправки";
        public static string AutoClickWhenHotKeyPressed { get; set; } = "Auto-click при нажатии горячей клавиши";
        public static string DetailTimeBefore { get; set; } = "Подробное время за";
        public static string AttacksUsedInTimer { get; set; } = "Атаки - используется в таймере";
        public static string ResultUsedInTimer { get; set; } = "Результат - используется в таймере";
        public static string Start { get; set; } = "Старт";
        public static string Basic { get; set; } = "Основные";
        public static string Signal { get; set; } = "Сигнал";
        public static string AutoClick { get; set; } = "Auto-Click";
        public static string DetailTime { get; set; } = "Детальное время";
        public static string UsedTable { get; set; } = "Используемая таблица";
        public static string HotKey { get; set; } = "Горячая клавиша";
        public static string EditTimeToolTip { get; set; } = "Редактировать время";
        public static string ChangeTo { get; set; } = "Изменить на";
        public static string HowToUse { get; set; } = "Как пользоваться";

        public static void SwitchToRUS()
        {
            HowToUse = "Как пользоваться";
            ChangeTo = "Изменить на";
            EditTimeToolTip = "Редактировать время";
            HotKey = "Горячая клавиша";
            UsedTable = "Используемая таблица";
            DetailTime = "Детальное время";
            AutoClick = "Auto-Click";
            Signal = "Сигнал";
            Basic = "Основные";
            Timer = "Таймер";
            Start = "Старт";
            ResultUsedInTimer = "Результат - используется в таймере";
            AttacksUsedInTimer = "Атаки - используется в таймере";
            DetailTimeBefore = "Подробное время за";
            TimerOptions = "Настройки таймера";
            Accept = "Принять";
            Cancel = "Отмена";
            Volume = "Громкость";
            HotKeyForStart = "Горячая клавиша для запуска таймера";
            UseAttacksFromTable = "Использовать атаки из таблицы";
            PlaySignalBefore = "Проиграть сигнал за";
            SecondsToSending = "секунд до отправки";
            AutoClickWhenHotKeyPressed  = "Auto-click при нажатии горячей клавиши";

            SendAttack = "Подготовить:";
            From = "Из";
            Updates = "Обновления";
            FAQ = "FAQ";
            Variants = "Вариантов";
            Abort = "Прервать";
            NotEnoughAttacks = "Требуется минимум 2 активные атаки.";
            CopyTextToBuffer = "Скопируйте текст в буфер обмена";
            No = "Нет";
            Active = "Активна";
            AboutProgram = "О программе";
            ToolTipDelta = "По умолчанию - 3.00";
            CreateNewFile = "Несохраненные данные будут утеряны, продолжить?";
            StatusBarText = "Не пропусти новый софт и свежие обновления ";
            NoSlotForCaptain = "Ни одна из атак не может быть капитаном. Измените тип атаки или выключите режим 'Обязателен капитан'";
            NoVariants = "Не найдено ни одного подходящего варианта. Примените карты или уменьшите время ожидания";
            ForceCaptain = "Обязателен капитан";
            UnsafeWarning = "Внимание! Изменение параметров может привести к погрешностям в вычислениях, перепроверяйте результаты!";
            Text  = "Текст";
            Filters = "Фильтры";
            Target = "Цель";
            TargetName = "Название цели";
            AttackName = "Название атаки";
            TypeArmy = "Тип армии";

            Name = "Название";
            Time = "Время";
            Type = "Тип";
            Card = "Карта";
            Add = "Добавить";
            Del = "Удалить";
            Cards = "Карты";
            Copy = "Копировать";

            Army = "Армия";
            Cap = "Капитан";

            ArmyType_Army = "Ударка";
            ArmyType_Captain = "Капитан";
            ArmyType_Unknown = "Неизвестно";

            RestTime = "Ожидание";
            Discipline = "Дисциплина";
            Recount = "Посчитать";

            //Menu
            File = "Файл";
            Help = "Помощь";
            Donate = "Поддержать проект";
            Lang = "Язык";
            New = "Новый";
            Open = "Открыть";
            Save = "Сохранить";
            SaveAs = "Сохранить как";
            Exit = "Выход";

            //GropBox
            Targets = "Цели";
            Attacks = "Атаки";
            Options = "Настройки";
            Result = "Результат";
            Unlock = "Разблокировать";

            Card2x = "Карты х2";
            Card3x = "Карты х3";
            Card5x = "Карты х5";
            Delta = "Дельта";
            TotalTimeToAttack = "Общее время для атаки";
            ExitMessage = "Вы уверены что хотите выйти?";

            InitArmyTypes();
            type_dict = new Dictionary<string, ArmyType>()
            {
                {ArmyType_Army,ArmyType.Army },
                {ArmyType_Captain,ArmyType.Captain },
                {ArmyType_Unknown,ArmyType.Unknown }
            };
            topics = new Dictionary<string, string>()
            {
                {Name,"NameDoc_RUS" },
                {Time,"TimeDoc_RUS" },
                {Type,"TypeDoc_RUS" },
                {Active,"ActiveDoc_RUS" },
                {Card,"CardDoc_RUS" },
                {RestTime,"RestTimeDoc_RUS" },
                {Targets,"TargetDoc_RUS" },
                {Copy,"CopyDoc_RUS" },
                {Unlock,"UnlockDoc_RUS" },
                {Donate,"DonateDoc_RUS" },
                {ForceCaptain,"ForceCaptainDoc_RUS" },
                {TotalTimeToAttack,"TotalTimeDoc_RUS" },
                {Updates,"UpdatesDoc_RUS" }
            };
            TimerTopics = new Dictionary<string, string>()
        {
                {HowToUse,"TimerHowToUseDoc_RUS" },
            {Signal,"TimerSignalDoc_RUS" },
            {AutoClick, "TimerAutoClickDoc_RUS" },
            {HotKey,"TimerHotKeyDoc_RUS" },
            {DetailTime,"TimerDetailTimeDoc_RUS" },
            {UsedTable,"TimerUsedTableDoc_RUS" }
        };
            CurrentLanguage = "RUS";
        }
        public static void SwitchToENG()
        {
            HowToUse = "How to use";
            ChangeTo = "Change to";
            EditTimeToolTip = "Edit time";
            HotKey = "Hot key";
            UsedTable = "The table used";
            DetailTime = "Detail time";
            AutoClick = "Auto-Click";
            Signal = "Signal";
            Basic = "Basic";
            Timer = "Timer";
            Start = "Start";
            ResultUsedInTimer = "Result - used in timer";
            AttacksUsedInTimer = "Attacks - used in timer";
            DetailTimeBefore = "Detail time";
            TimerOptions = "Timer settings";
            Accept = "Accept";
            Cancel = "Cancel";
            Volume = "Volume";
            HotKey = "HotKey to start the timer";
            UseAttacksFromTable = "Use attacks from the table";
            PlaySignalBefore = "Play signal";
            SecondsToSending = "seconds before sending";
            AutoClickWhenHotKeyPressed = "Auto-click when hot key is pressed";

            SendAttack = "Send attack:";
            From = "From";
            Updates = "Updates";
            FAQ = "FAQ";
            Variants = "Variants";
            Abort = "Abort";
            NotEnoughAttacks = "Requires at least 2 attacks";
            CopyTextToBuffer = "Copy text to the clipboard";
            No = "No";
            Active = "Is Active";
            AboutProgram = "About";
            ToolTipDelta = "Default - 3.00";
            CreateNewFile = "Unsaved data will be lost, continue?";
            StatusBarText = "Do not miss the new soft and latest updates";
            NoSlotForCaptain = "There are no free slots for captain. Change the type of attack or turn off the 'Only with Captain'";
            NoVariants = "No result. Apply cards or reduce 'Rest time'";
            ForceCaptain = "Only with Captain";
            UnsafeWarning = "Warning! Changing the parameters can lead to errors in the calculations, recheck the results!";
            Text = "Text";
            Filters = "Filters";
            Target = "Target";
            TargetName = "Target name";
            AttackName = "Attack name";
            TypeArmy = "Army type";

            Name = "Name";
            Time = "Time";
            Type = "Type";
            Card = "Card";
            Add = "Add";
            Del = "Delete";
            Cards = "Cards";
            Copy = "Copy";

            Army = "Army";
            Cap = "Captain";

            ArmyType_Army = "Attack";
            ArmyType_Captain = "Captain";
            ArmyType_Unknown = "Unknown";

            RestTime = "Rest time";
            Discipline = "Discipline";
            Recount = "Count";

            //Menu
            File = "File";
            Help = "Help";
            Donate = "Support the project";
            Lang = "Language";
            New = "New";
            Open = "Open";
            Save = "Save";
            SaveAs = "Save as";
            Exit = "Exit";

            //GropBox
            Targets = "Targets";
            Attacks = "Attacks";
            Options = "Options";
            Result = "Result";
            Unlock = "Unlock";

            Card2x = "Cards х2";
            Card3x = "Cards х3";
            Card5x = "Cards х5";
            Delta = "Delta";
            TotalTimeToAttack = "Total time to attack";
            ExitMessage = "Are you sure you want to quit?";

            type_dict = new Dictionary<string, ArmyType>()
            {
                {ArmyType_Army,ArmyType.Army },
                {ArmyType_Captain,ArmyType.Captain },
                {ArmyType_Unknown,ArmyType.Unknown }
            };
            InitArmyTypes();
            topics = new Dictionary<string, string>()
            {
                {Name,"NameDoc_ENG" },
                {Time,"TimeDoc_ENG" },
                {Type,"TypeDoc_ENG" },
                {Active,"ActiveDoc_ENG" },
                {Card,"CardDoc_ENG" },
                {RestTime,"RestTimeDoc_ENG" },
                {Targets,"TargetDoc_ENG" },
                {Copy,"CopyDoc_ENG" },
                {Unlock,"UnlockDoc_ENG" },
                {Donate,"DonateDoc_ENG" },
                {ForceCaptain,"ForceCaptainDoc_ENG" },
                {TotalTimeToAttack,"TotalTimeDoc_ENG" },
                {Updates,"UpdatesDoc_ENG" }
            };
            TimerTopics = new Dictionary<string, string>()
        {
                {HowToUse,"TimerHowToUseDoc_ENG" },
            {Signal,"TimerSignalDoc_ENG" },
            {AutoClick, "TimerAutoClickDoc_ENG" },
            {HotKey,"TimerHotKeyDoc_ENG" },
            {DetailTime,"TimerDetailTimeDoc_ENG" },
            {UsedTable,"TimerUsedTableDoc_ENG" }
        };
            CurrentLanguage = "ENG";
        }

        private static void InitArmyTypes()
        {
            types[0] = ArmyType_Army;
            types[1] = ArmyType_Captain;
            types[2] = ArmyType_Unknown;
        }

        public static ObservableCollection<string> types = new ObservableCollection<string>() { ArmyType_Army, ArmyType_Captain, ArmyType_Unknown };
        public static Dictionary<string, ArmyType> type_dict = new Dictionary<string, ArmyType>()
        {
            {ArmyType_Army,ArmyType.Army },
            {ArmyType_Captain,ArmyType.Captain },
            {ArmyType_Unknown,ArmyType.Unknown },
        };
        public static Dictionary<string, string> topics = new Dictionary<string, string>()
        {
            {Name,"NameDoc_RUS" },
            {Time,"TimeDoc_RUS" },
            {Type,"TypeDoc_RUS" },
            {Active,"ActiveDoc_RUS" },
            {Card,"CardDoc_RUS" },
            {RestTime,"RestTimeDoc_RUS" },
            {Targets,"TargetDoc_RUS" },
            {Copy,"CopyDoc_RUS" },
            {Unlock,"UnlockDoc_RUS" },
            {Donate,"DonateDoc_RUS" },
            {ForceCaptain,"ForceCaptainDoc_RUS" },
            {TotalTimeToAttack,"TotalTimeDoc_RUS" },
            {Updates,"UpdatesDoc_RUS" }
        };
        public static Dictionary<string, string> TimerTopics = new Dictionary<string, string>()
        {
            {HowToUse,"TimerHowToUseDoc_RUS" },
            {Signal,"TimerSignalDoc_RUS" },
            {AutoClick, "TimerAutoClickDoc_RUS" },
            {HotKey,"TimerHotKeyDoc_RUS" },
            {DetailTime,"TimerDetailTimeDoc_RUS" },
            {UsedTable,"TimerUsedTableDoc_RUS" }
        };
    }
}
