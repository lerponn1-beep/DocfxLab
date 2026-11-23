using System;

namespace RailwayControl
{
    /// @defgroup SystemEnums Перечисления системы
    /// @brief Автогенерируемая страница со всеми перечислениями (enums) проекта.
    /// @details Содержит полный перечень состояний светофоров, режимов движения поездов
    /// и приоритетов маршрутов с кастомными пояснениями и ограничениями для каждого значения.

    /// <summary>
    /// Показания путевого светофора.
    /// </summary>
    /// <remarks>
    /// Используется диспетчерской системой для регулирования скорости на блок-участке.
    /// </remarks>
    /// @ingroup SystemEnums
    public enum SignalAspect
    {
        /// <summary>
        /// Запрещающий сигнал.
        /// </summary>
        /// @warning Проезд сигнала категорически запрещен. Требуется полная остановка.
        Red = 0,

        /// <summary>
        /// Предупреждающий сигнал.
        /// </summary>
        /// @note Разрешается движение с готовностью остановиться; следующий светофор закрыт.
        Yellow = 1,

        /// <summary>
        /// Разрешающий сигнал.
        /// </summary>
        /// @note Путь свободен, впереди свободно не менее двух блок-участков. Движение с установленной скоростью.
        Green = 2,

        /// <summary>
        /// Пригласительный сигнал, лунно-белый мигающий.
        /// </summary>
        /// @warning Разрешает проезд запрещающего сигнала со скоростью не более 20 км/ч с особой бдительностью.
        FlashingWhite = 3
    }

    /// <summary>
    /// Текущий эксплуатационный статус поезда.
    /// </summary>
    /// @ingroup SystemEnums
    public enum TrainState
    {
        /// <summary>
        /// Поезд находится на станции в ожидании отправления.
        /// </summary>
        /// @note Двигатели в режиме холостого хода, двери могут быть открыты.
        Idle,

        /// <summary>
        /// Движение по перегону в штатном режиме.
        /// </summary>
        /// @note Скорость контролируется автоматически согласно <see cref="SignalAspect"/>.
        Running,

        /// <summary>
        /// Применено служебное или экстренное торможение.
        /// </summary>
        /// @warning При экстренном торможении автоматически отправляется уведомление диспетчеру.
        Braking,

        /// <summary>
        /// Аварийная остановка или неисправность систем.
        /// </summary>
        /// @warning Требуется немедленное закрытие перегона и вызов вспомогательного локомотива.
        Emergency
    }

    /// <summary>
    /// Категория приоритета маршрута.
    /// </summary>
    /// @ingroup SystemEnums
    public enum RoutePriority
    {
        /// <summary>Высший приоритет, скоростные пассажирские поезда.</summary>
        /// @note Имеет безусловное преимущество при проследовании узловых станций.
        High = 1,

        /// <summary>Стандартный приоритет, обычные пассажирские и пригородные поезда.</summary>
        Normal = 2,

        /// <summary>Низкий приоритет, грузовые и хозяйственные составы.</summary>
        /// @note Может быть поставлен под обгон на боковой путь.
        Low = 3
    }

    /// <summary>
    /// Структура телеметрии подвижного состава.
    /// </summary>
    /// <remarks>
    /// Передается по значению для фиксации мгновенного снимка параметров движения.
    /// </remarks>
    public struct TrainTelemetry
    {
        /// <summary>Текущая скорость состава, км/ч.</summary>
        public double SpeedKmh;

        /// <summary>Координата на участке, пикетаж в метрах.</summary>
        public double PositionMeters;

        /// <summary>Текущее состояние поезда.</summary>
        public TrainState State;

        /// <summary>
        /// Создает новый снимок телеметрии.
        /// </summary>
        /// <param name="speedKmh">Скорость в км/ч.</param>
        /// <param name="positionMeters">Координата в метрах.</param>
        /// <param name="state">Статус из перечисления <see cref="TrainState"/>.</param>
        public TrainTelemetry(double speedKmh, double positionMeters, TrainState state)
        {
            SpeedKmh = speedKmh;
            PositionMeters = positionMeters;
            State = state;
        }

        /// <summary>
        /// Проверяет, находится ли состав в движении.
        /// </summary>
        /// <returns><c>true</c>, если скорость больше нуля; иначе <c>false</c>.</returns>
        public bool IsMoving() => SpeedKmh > 0.1;
    }
}
