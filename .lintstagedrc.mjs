import micromatch from 'micromatch'

export default {
    '*.+(csproj|cs|razor|html|css|js|json)': (files) => {
        const match = micromatch.not(files, ['package.json', 'package-lock.json'])
        if (match.length === 0) {
            return ""
        }

        return `jb cleanupcode Couple.sln --include=${match.join(";")}`
    }
}
