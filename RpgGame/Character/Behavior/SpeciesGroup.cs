using System;
using System.Collections.Generic;

namespace RpgGame.Character.Behavior
{
    public sealed class SpeciesGroup<TMember> where TMember : class, ISpeciesMember
    {
        private readonly List<TMember> members = new();
        public void Register(TMember member)
        {
            ArgumentNullException.ThrowIfNull(member);
            if (!members.Contains(member))
            {
                members.Add(member);
            }

        }

        public void Unregister(TMember member)
        {
            ArgumentNullException.ThrowIfNull(member);
            members.Remove(member);
        }

        public void NotifyMemberDeath(TMember fallenMember)
        {
            ArgumentNullException.ThrowIfNull(fallenMember);
            var snapshot = members.ToArray();
            foreach (var member in snapshot)
            {
                if (!ReferenceEquals(member, fallenMember))
                {
                    member.OnSpeciesMemberDeath();
                }
            }
        }
    }
}
