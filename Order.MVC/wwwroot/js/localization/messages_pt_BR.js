// wwwroot/js/localization/messages_pt_BR.js
(function(factory) {
    if (typeof define === "function" && define.amd) {
        define(["jquery", "../jquery.validate"], factory);
    } else if (typeof module === "object" && module.exports) {
        module.exports = factory(require("jquery"));
    } else {
        factory(jQuery);
    }
}(function($) {
    $.extend($.validator.messages, {
        required: "Este campo é obrigatório.",
        remote: "Por favor, corrija este campo.",
        email: "Por favor, forneça um endereço de email válido.",
        url: "Por favor, forneça uma URL válida.",
        date: "Por favor, forneça uma data válida.",
        dateISO: "Por favor, forneça uma data válida (ISO).",
        number: "Por favor, forneça um número válido.",
        digits: "Por favor, forneça somente dígitos.",
        creditcard: "Por favor, forneça um cartão de crédito válido.",
        equalTo: "Por favor, forneça o mesmo valor novamente.",
        maxlength: $.validator.format("Por favor, forneça não mais que {0} caracteres."),
        minlength: $.validator.format("Por favor, forneça ao menos {0} caracteres."),
        rangelength: $.validator.format("Por favor, forneça um valor entre {0} e {1} caracteres."),
        range: $.validator.format("Por favor, forneça um valor entre {0} e {1}."),
        max: $.validator.format("Por favor, forneça um valor menor ou igual a {0}."),
        min: $.validator.format("Por favor, forneça um valor maior ou igual a {0}.")
    });
    return $;
}));
