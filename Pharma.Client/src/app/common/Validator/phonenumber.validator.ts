import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function phoneNumberValidator(defaultCountry: string): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {
        var value = control.value ? control.value.toString() : '';

        if (!value) {
            return null;
        }
        const patterns = {
            '+91': /^(\+91[ \-]?|91[ \-]?|0)?[6-9]\d{9}$/,
            '+880': /^(\+880|880)?1[3-9]\d{8}$/,
            '+1': /^(\+1|1)?\d{10}$/,
        };

        if (!value.startsWith('+')) {
            value = defaultCountry + value;
        }
        for (const [code, pattern] of Object.entries(patterns)) {
            if (value.startsWith(code) && !pattern.test(value)) {
                return { invalidPhoneNumber: true };
            }
        }

        return null;
    };
}

