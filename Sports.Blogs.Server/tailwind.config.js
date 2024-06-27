const colors = require('tailwindcss/colors')
module.exports = {
    darkMode: 'class',
    theme: {
        extend: {
            colors: {
                customBlue: '#22c55e'
            }
        }
    },
    variants: {
        extend: {
            backgroundColor: ['high-contrast'],
        },
    },
    content: ["./**/*.razor", "./wwwroot/index.html"],
    plugins: [
        require('@tailwindcss/forms'),
    ]
}