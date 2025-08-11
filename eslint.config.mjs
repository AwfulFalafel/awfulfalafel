import globals from "globals";
import pluginJs from "@eslint/js";
import tseslint from "typescript-eslint";
import stylistic from "@stylistic/eslint-plugin";

export default [
	{
		plugins: {
			"@stylistic": stylistic
		}
	},
	{ files: ["**/*.{js,mjs,cjs,ts}"] },
	{
		ignores: ["dist/**/*"]
	},
	{ languageOptions: { globals: globals.browser } },
	pluginJs.configs.recommended,
	...tseslint.configs.recommended,
	{
		rules: {
			"@stylistic/semi": ["error", "always"],
			"@stylistic/quotes": ["error", "double", { "allowTemplateLiterals": "always" }],
			"@stylistic/indent": ["error", "tab"],
			"@stylistic/no-extra-semi": "error",
			"@stylistic/array-bracket-spacing": ["error", "never"],
			"@stylistic/keyword-spacing": ["error", { "before": true, "after": true }],
			"@stylistic/no-multiple-empty-lines": ["error", { "max": 1, "maxEOF": 1 }],
			"@stylistic/object-curly-spacing": ["error", "always"],
			"@stylistic/space-before-blocks": "error",
			"@stylistic/brace-style": ["error", "stroustrup"],
			"@typescript-eslint/ban-ts-comment": "off",
			"@typescript-eslint/no-explicit-any": "off",
			"@typescript-eslint/no-require-imports": "off",
			"@typescript-eslint/no-unsafe-function-type": "off",
			"@typescript-eslint/no-unused-expressions": "off",
			"@typescript-eslint/no-unused-vars": "off",
			"no-constant-binary-expression": "off",
			"no-dupe-keys": "off",
			"no-empty": "off",
			"no-undef": "off",
			"prefer-const": "off",
		}
	}
];
