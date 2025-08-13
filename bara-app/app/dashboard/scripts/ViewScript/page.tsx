import React from "react";
import ScriptNavbar from "@/components/ScriptNavbar";

const ViewScript = () => {
  return (
    <div className="h-screen flex flex-col">
      {/* Navbar */}
      <ScriptNavbar />

      {/* Content Area */}
      <div className="flex-1 bg-white flex items-center justify-center">
        <p className="text-gray-500">still working on this page</p>
      </div>
    </div>
  );
};

export default ViewScript;
