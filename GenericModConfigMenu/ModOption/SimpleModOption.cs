using System;
using GenericModConfigMenu.Framework;

namespace GenericModConfigMenu.ModOption
{
    internal class SimpleModOption<T> : BaseModOption
    {
        public Type Type { get; }
        protected Func<T> Getter;
        protected Action<T> Setter;

        private T State;
        public virtual T Value
        {
            get => this.State;
            set
            {
                if (!this.State.Equals(value))
                    this.Owner.Options[this.Owner.ActiveDisplayPage.Name].ChangeHandler.ForEach(c => c.Invoke(this.Id, value));

                this.State = value;
            }
        }

        public override void SyncToMod()
        {
            this.State = this.Getter.Invoke();
        }

        public override void Save()
        {
            SpaceShared.Log.Trace("saving " + this.Name + " " + this.Description);
            this.Setter.Invoke(this.State);
        }

        public SimpleModOption(string name, string desc, Type type, Func<T> theGetter, Action<T> theSetter, string id, ModConfig mod)
            : base(name, desc, id, mod)
        {
            this.Type = type;
            this.Getter = theGetter;
            this.Setter = theSetter;

            this.State = this.Getter.Invoke();
        }
    }
}
