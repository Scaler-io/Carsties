"use client";

import { Spinner } from "flowbite-react";
import React from "react";

const Loader = () => {
  return (
    <div className="h-[45vh] flex flex-col gap-y-5 justify-center items-center">
      <Spinner color="info" size="xl" />
      <h3>Please wait</h3>
    </div>
  );
};

export default Loader;
