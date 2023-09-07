const pluginRss = require("@11ty/eleventy-plugin-rss")

module.exports = (config) => {
    // Add Eleventy RSS plugin
    config.addPlugin(pluginRss)

    // Copy the fonts and images folders without modification
    config.addPassthroughCopy("src/assets/fonts")
    config.addPassthroughCopy("src/assets/images")

    // Add styles.css to dev watch targets so we get automatic reload
    // if the base stylesheet changes.
    config.addWatchTarget("./src/assets/css/styles.css")

    return {
        dir: {
            input: "src",
            output:"dist",
            includes: "_includes",
            layouts: "_layouts"
        },
        htmlTemplateEngine: "ejs"
    }
}
