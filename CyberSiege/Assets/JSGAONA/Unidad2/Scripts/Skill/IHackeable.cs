namespace Assets.JSGAONA.Unidad2.Scripts.Skill {

    public interface IHackeable {
        
        bool ItsHacked { get; set; }

        // Metodo que permite hackear
        void Hack(float timeHack);

    }
}