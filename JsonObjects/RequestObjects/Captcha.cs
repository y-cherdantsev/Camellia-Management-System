﻿namespace Camellia_Management_System.JsonObjects.RequestObjects
{
    /// @author Yevgeniy Cherdantsev
    /// @date 14.05.2020 16:21:40
    /// @version 1.0
    /// <summary>
    /// Captcha object that should be send to camellia system
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="captchaSolution">Solved captcha</param>
        public Captcha(string captchaSolution)
        {
            captchaCode = captchaSolution;
        }

        public string captchaCode { get; set; }
    }
}