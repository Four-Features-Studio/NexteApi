namespace NexteAPI.Configs
{
    public class TexturesOptions
    {
        /// <summary>
        /// Путь до скинов
        /// </summary>
        public string PathSkin { get; set; }
        /// <summary>
        /// Путь до плащей
        /// </summary>
        public string PathCloak { get; set; }
        /// <summary>
        /// Дефолтный скин
        /// </summary>
        public bool DefaultSkinEnabled { get; set; } = true;
        /// <summary>
        /// Название дефолтного скина
        /// </summary>
        public string DefaultSkinName { get; set; }

        /// <summary>
        /// Дефолтный плащ
        /// </summary>
        public bool DefaultCloakEnabled { get; set; } = false;
        /// <summary>
        /// Название дефолтного плаща
        /// </summary>
        public string DefaultCloakName { get; set; }
    }
}
