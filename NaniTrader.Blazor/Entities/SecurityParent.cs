﻿namespace NaniTrader.Entities
{
    public class SecurityParent
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public ParentType ParentType { get; set; }
    }
}
