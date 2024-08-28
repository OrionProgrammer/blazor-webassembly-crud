/** @type {import('tailwindcss').Config} */

const colors = require('tailwindcss/colors')

module.exports = {
    content: [
        './Components/**/*.razor',
        './Layout/**/*.razor',
        './Pages/**/*.razor',
    ],
    theme: {
        extend: {
            colors,
        },
    },
    plugins: [
        require('@tailwindcss/typography'),
    ],
}