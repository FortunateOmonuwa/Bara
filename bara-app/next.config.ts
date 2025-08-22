import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  reactStrictMode: true,

  // ✨ This disables the floating dev overlay/toolbox
  devIndicators: {
    buildActivity: false,
  },

  // Enable standalone output for Docker
  output: "standalone",

  // Optimize for production
  experimental: {
    optimizePackageImports: ["react-hot-toast", "react-pdf"],
  },

  // Configure image optimization
  images: {
    unoptimized: false,
    remotePatterns: [
      {
        protocol: "https",
        hostname: "**",
      },
    ],
  },
};

export default nextConfig;
