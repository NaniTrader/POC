using Volo.Abp.Domain.Entities;
using Volo.Abp;

namespace NaniTrader.Entities.Misc
{
    public class Country : BasicAggregateRoot<Guid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        // here for ef core
        private Country() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public string Name { get; private set; }

        internal Country(Guid id, string name) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CountryConsts.MaxNameLength);
        }
    }
}
