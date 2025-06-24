// wwwroot/js/currency-input.js
$(document).ready(function () {

    $.validator.methods.number = function (value, element) {
        value = value.replace(",", ".");
        return this.optional(element) || !isNaN(value);
    };

    // Verifica se a biblioteca Inputmask está disponível
    if (typeof $.fn.inputmask !== 'undefined') {
        // Configura o Inputmask para campos monetários
        $(".money-input").inputmask('decimal', {
            alias: 'numeric',
            radixPoint: ',',
            groupSeparator: '.',
            autoGroup: true,
            digits: 2,
            digitsOptional: false,
            prefix: '',
            placeholder: '0',
            rightAlign: false,
            allowMinus: false,
            onBeforeMask: function(value, opts) {
                // Remove formatação R$ se presente
                return value ? value.replace("R$", "").trim() : value;
            }
        });

        // Configura o submit do formulário para converter o formato brasileiro para o padrão esperado pelo .NET
        $('form').on('submit', function() {
            $('.money-input').each(function() {
                var input = $(this);
                var value = input.val();
                
                if (value) {
                    // Remove pontos e substitui vírgula por ponto (formato brasileiro para formato .NET)
                    var numericValue = value.replace(/\./g, '').replace(',', '.');
                    input.val(numericValue);
                }
            });
            return true;
        });

        // Complementando o jQuery Validator para aceitar números com vírgula (formato brasileiro)
        extendValidatorForBrazilianCurrency();
    } else {
        console.error("A biblioteca Inputmask não foi carregada. Verifique se ela está incluída no projeto.");
    }

    // Função para estender o validador jQuery para aceitar números no formato brasileiro
    function extendValidatorForBrazilianCurrency() {
        if ($.validator) {
            // Método para validar números no formato brasileiro
            $.validator.addMethod(
                "brazilianNumber",
                function(value, element) {
                    // Aceita vazio (para campos não obrigatórios)
                    if (this.optional(element)) return true;
                    
                    // Testa se é um número válido no formato brasileiro
                    return /^(?:\d+|\d{1,3}(?:\.\d{3})+)(?:,\d+)?$/.test(value);
                },
                "Por favor, insira um número válido."
            );

            // Substitui o validador padrão de número para campos com a classe money-input
            $.validator.addClassRules("money-input", {
                brazilianNumber: true
            });
        }
    }
});
