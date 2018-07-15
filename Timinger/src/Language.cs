using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timinger
{
    class Language
    {
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
        public static string Recount { get; set; } = "Просчитать";

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

        public static string TotalTimeToAttack { get; set; } = "Общее время для атаки: ";
        public static string ExitMessage { get; set; } = "Вы уверены что хотите выйти?";
        public static string CreateNewFile { get; set; } = "Несохраненные данные будут утеряны, продолжить?";
        public static string StatusBarText { get; set; } = "Текст статус бара";
        public static string ForceCaptain { get; set; } = "Обязателен капитан";
        public static string NoSlotForCaptain { get; set; } = "Ни одна из атак не может быть капитаном. Измените тип атаки или выключите режим 'Обязателен капитан'";
        public static string NoVariants { get; set; } = "Не найдено ни одного подходящего варианта. Примените карты или уменьшите время ожидания";

        public static void SwitchToRUS()
        {
            CreateNewFile = "Несохраненные данные будут утеряны, продолжить?";
            StatusBarText = "Текст статус бара";
            NoSlotForCaptain = "Ни одна из атак не может быть капитаном. Измените тип атаки или выключите режим 'Обязателен капитан'";
            NoVariants = "Не найдено ни одного подходящего варианта. Примените карты или уменьшите время ожидания";
            ForceCaptain = "Обязателен капитан";
            Target = "Цель";

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
            Recount = "Просчитать";

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
            TotalTimeToAttack = "Общее время для атаки: ";
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
                {Card,"CardDoc_RUS" },
                {RestTime,"RestTimeDoc_RUS" },
                {Targets,"TargetDoc_RUS" },
                {Copy,"CopyDoc_RUS" },
                {Unlock,"UnlockDoc_RUS" },
                {Donate,"DonateDoc_RUS" },
                {ForceCaptain,"ForceCaptainDoc_RUS" }
            };
            CurrentLanguage = "RUS";
        }
        public static void SwitchToENG()
        {
            NoSlotForCaptain = "No slots for captain. Change army types or turn off 'Force captain'";
            StatusBarText = "Status bar text";
            NoVariants = "No variants. Apply several cards or reduce rest time";
            ForceCaptain = "Force captain";
            Target = "Target";
            CreateNewFile = "Unsaved data will be lost, continue?";
            Name = "Name";
            Time = "Time";
            Type = "Type";
            Card = "Card";
            Add = "Add";
            Del = "Del";
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
            Donate = "Support project";
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
            TotalTimeToAttack = "Total time for attack: ";
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
                {Card,"CardDoc_ENG" },
                {RestTime,"RestTimeDoc_ENG" },
                {Targets,"TargetDoc_ENG" },
                {Copy,"CopyDoc_ENG" },
                {Unlock,"UnlockDoc_ENG" },
                {Donate,"DonateDoc_ENG" },
                {ForceCaptain,"ForceCaptainDoc_ENG" }
            };
            CurrentLanguage = "ENG";
        }

        private static void InitArmyTypes()
        {
            types[0] = ArmyType_Army;
            types[1] = ArmyType_Captain;
            types[2] = ArmyType_Unknown;
        }

        public static ObservableCollection<string> types = new ObservableCollection<string>() { ArmyType_Army + "!", ArmyType_Captain, ArmyType_Unknown };
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
            {Card,"CardDoc_RUS" },
            {RestTime,"RestTimeDoc_RUS" },
            {Targets,"TargetDoc_RUS" },
            {Copy,"CopyDoc_RUS" },
            {Unlock,"UnlockDoc_RUS" },
            {Donate,"DonateDoc_RUS" },
            {ForceCaptain,"ForceCaptainDoc_RUS" }
        };
    }
}
