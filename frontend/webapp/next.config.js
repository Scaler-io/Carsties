/** @type {import('next').NextConfig} */
const nextConfig = {
    images:{
        remotePatterns: [{
            hostname: 'cdn.pixabay.com'
        },
        {
            hostname: "wallpapercave.com"
        }]
    }
}

module.exports = nextConfig
