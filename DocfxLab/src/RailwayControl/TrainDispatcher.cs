using System;

namespace RailwayControl
{
    /// <summary>
    /// Диспетчер управления движением на участке.
    /// </summary>
    /// <remarks>
    /// Координирует показания светофоров (<see cref="SignalAspect"/>) и рассчитывает
    /// допустимые скорости с учетом приоритета (<see cref="RoutePriority"/>).
    /// </remarks>
    public class TrainDispatcher
    {
        /// <summary>Идентификатор контролируемого участка.</summary>
        public string SectionId { get; }

        /// <summary>Текущее показание входного светофора.</summary>
        public SignalAspect CurrentSignal { get; private set; }

        /// <summary>Приоритет текущего маршрута.</summary>
        public RoutePriority ActivePriority { get; set; }

        /// <summary>
        /// Инициализирует диспетчера для заданного участка пути.
        /// </summary>
        /// <param name="sectionId">Код участка.</param>
        /// <param name="priority">Начальный приоритет маршрута.</param>
        public TrainDispatcher(string sectionId, RoutePriority priority = RoutePriority.Normal)
        {
            SectionId = sectionId;
            ActivePriority = priority;
            CurrentSignal = SignalAspect.Red;
        }

        /// <summary>
        /// Обновляет показание светофора на основе телеметрии поезда.
        /// </summary>
        /// <param name="telemetry">Снимок телеметрии <see cref="TrainTelemetry"/>.</param>
        /// <returns>Новое установленное показание светофора.</returns>
        /// @see SignalAspect
        /// @see TrainState
        public SignalAspect UpdateSignal(TrainTelemetry telemetry)
        {
            if (telemetry.State == TrainState.Emergency)
            {
                CurrentSignal = SignalAspect.Red;
            }
            else if (telemetry.State == TrainState.Braking)
            {
                CurrentSignal = SignalAspect.Yellow;
            }
            else
            {
                CurrentSignal = SignalAspect.Green;
            }

            return CurrentSignal;
        }

        /// <summary>
        /// Рассчитывает максимально допустимую скорость по текущему сигналу.
        /// </summary>
        /// <returns>Допустимая скорость в км/ч.</returns>
        /// @note Для <see cref="RoutePriority.High"/> при зеленом сигнале лимит повышен до 140 км/ч.
        public double GetMaxAllowedSpeed()
        {
            return CurrentSignal switch
            {
                SignalAspect.Red => 0.0,
                SignalAspect.FlashingWhite => 20.0,
                SignalAspect.Yellow => 60.0,
                SignalAspect.Green => ActivePriority == RoutePriority.High ? 140.0 : 100.0,
                _ => 0.0
            };
        }

        /// <summary>
        /// Формирует текстовый отчет о состоянии участка.
        /// </summary>
        /// <param name="telemetry">Данные телеметрии.</param>
        /// <returns>Строка сводки для журнала диспетчера.</returns>
        public string BuildStatusReport(TrainTelemetry telemetry)
        {
            double maxSpeed = GetMaxAllowedSpeed();
            return $"[{SectionId}] Сигнал: {CurrentSignal}, Статус: {telemetry.State}, Лимит: {maxSpeed} км/ч";
        }
    }
}
