import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  reactStrictMode: true,

  // ✨ This disables the floating dev overlay/toolbox
  devIndicators: {
    buildActivity: false,
  },
};

export default nextConfig;
