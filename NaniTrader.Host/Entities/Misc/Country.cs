using Volo.Abp.Domain.Entities;
using Volo.Abp;
using NaniTrader.Entities.Misc.Shared;

namespace NaniTrader.Entities.Misc
{
    public class Country : BasicAggregateRoot<Ulid>
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        // here for ef core
        private Country() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor.
        public string Name { get; private set; }

        internal Country(Ulid id, string name) : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name), CountryConsts.MaxNameLength);
        }
    }
}
