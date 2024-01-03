import React from "react";
import Filter from "./filter-actions";
import SortActions from "./sort-actions";
import PagingActions from "./paging-actions";

const PageActions = () => {
  return (
    <div className="flex justify-between items-center mb-4">
      <Filter />
      <SortActions />
      <PagingActions />
    </div>
  );
};

export default PageActions;
